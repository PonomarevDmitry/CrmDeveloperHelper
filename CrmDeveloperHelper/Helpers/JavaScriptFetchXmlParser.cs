using Jint;
using Jint.Parser;
using Jint.Parser.Ast;
using Microsoft.VisualStudio.Text.Editor;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Helpers
{
    public class JavaScriptFetchXmlParser
    {
        private readonly string _javaScriptCode;
        private readonly IWriteToOutput _iWriteToOutput;

        public JavaScriptFetchXmlParser(IWriteToOutput iWriteToOutput, string javaScriptCode)
        {
            this._javaScriptCode = javaScriptCode;
            this._iWriteToOutput = iWriteToOutput;
        }

        public void PasteFetchXml(IWpfTextView wpfTextView, int oldCaretLine, int oldCaretColumn)
        {
            string fetchXml = ParseJavaScriptCode();

            if (string.IsNullOrEmpty(fetchXml))
            {
                return;
            }

            using (var edit = wpfTextView.TextBuffer.CreateEdit())
            {
                int position = edit.Snapshot.GetLineFromLineNumber(oldCaretLine).Start + oldCaretColumn;

                edit.Insert(position, fetchXml);

                edit.Apply();
            }
        }

        private string ParseJavaScriptCode()
        {
            JavaScriptParser parser = new JavaScriptParser();

            var prog = parser.Parse(this._javaScriptCode);

            var engine = new Engine();

            bool resultFillParameters = FillParameters(engine, prog, out var varName);

            bool executed = false;

            int count = 0;

            while (!executed && count < 1000)
            {
                try
                {
                    engine.Execute(prog);

                    executed = true;
                }
                catch (Jint.Runtime.JavaScriptException ex)
                {
                    count++;

                    bool exceptionHandled = false;

                    if (ex.Error.IsObject())
                    {
                        var obj = ex.Error.AsObject();

                        if (string.Equals(obj.Class, "Error", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var name = obj.GetOwnProperty("name");
                            var message = obj.GetOwnProperty("message");

                            if (name != null && message != null && name.Value.IsString() && message.Value.IsString())
                            {
                                var nameValue = name.Value.AsString();
                                var messageValue = message.Value.AsString();

                                exceptionHandled = HandleException(engine, nameValue, messageValue);
                            }
                        }
                    }

                    if (!exceptionHandled)
                    {
                        _iWriteToOutput.WriteErrorToOutput(null, ex);

                        executed = false;

                        break;
                    }
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(null, ex);

                    executed = false;

                    break;
                }
            }

            string result = string.Empty;

            if (executed)
            {
                var lastVar = engine.Global.GetOwnProperty(varName);

                if (lastVar != null)
                {
                    result = lastVar.Value.AsString();

                    if (!string.IsNullOrEmpty(result))
                    {
                        var commonConfig = Model.CommonConfiguration.Get();

                        result = ContentComparerHelper.FormatXmlByConfiguration(
                            result
                            , commonConfig
                            , XmlOptionsControls.SavedQueryXmlOptions
                            , schemaName: Commands.AbstractDynamicCommandXsdSchemas.FetchSchema
                        );
                    }
                }
            }

            return result;
        }

        private static bool HandleException(Engine engine, string nameValue, string messageValue)
        {
            if (string.Equals(nameValue, "ReferenceError", StringComparison.InvariantCultureIgnoreCase)
                && messageValue.EndsWith(" is not defined")
            )
            {
                var missedValue = messageValue.Split(' ')[0];

                if (!string.IsNullOrEmpty(missedValue))
                {
                    engine.SetValue(missedValue, new Jint.Native.JsValue("@" + missedValue));

                    return true;
                }
            }

            return false;
        }

        private static bool FillParameters(Engine engine, Jint.Parser.Ast.Program prog, out string varName)
        {
            varName = string.Empty;

            var identifiers = new Dictionary<string, SyntaxNodes>(StringComparer.InvariantCultureIgnoreCase);

            if (prog.VariableDeclarations.Any(d => d.Declarations.Any()))
            {
                var variableDecl = prog.VariableDeclarations.Last(d => d.Declarations.Any());

                varName = variableDecl.Declarations.LastOrDefault().Id.Name;
            }

            foreach (var item in prog.Body)
            {
                GetIndenfiers(identifiers, item);
            }

            foreach (var item in identifiers.Keys)
            {
                var type = identifiers[item];

                if (type == SyntaxNodes.Identifier)
                {
                    engine.SetValue(item, new Jint.Native.JsValue("@" + item));
                }
                else if (type == SyntaxNodes.ArrayExpression)
                {
                    var array = engine.Array.Construct(new[] { new Jint.Native.JsValue("@" + item + "1"), new Jint.Native.JsValue("@" + item + "2"), new Jint.Native.JsValue("@" + item + "3") });

                    engine.SetValue(item, new Jint.Native.JsValue(array));
                }
            }

            return true;
        }

        private static void GetIndenfiers(Dictionary<string, SyntaxNodes> identifiers, Statement statement)
        {
            if (statement is VariableDeclaration variableDeclaration)
            {
                foreach (var decl in variableDeclaration.Declarations)
                {
                    GetIndenfiers(identifiers, decl.Init);
                }
            }
            else if (statement is ForStatement forStatement)
            {
                GetIndenfiers(identifiers, forStatement.Update);
                GetIndenfiers(identifiers, forStatement.Test);

                GetIndenfiers(identifiers, forStatement.Body);
            }
            else if (statement is BlockStatement blockStatement)
            {
                foreach (var item in blockStatement.Body)
                {
                    GetIndenfiers(identifiers, item);
                }
            }
            else if (statement is DoWhileStatement doWhileStatement)
            {
                GetIndenfiers(identifiers, doWhileStatement.Test);
                GetIndenfiers(identifiers, doWhileStatement.Body);
            }
            else if (statement is WhileStatement whileStatement)
            {
                GetIndenfiers(identifiers, whileStatement.Body);
                GetIndenfiers(identifiers, whileStatement.Test);
            }
            else if (statement is ExpressionStatement expressionStatement)
            {
                GetIndenfiers(identifiers, expressionStatement.Expression);
            }
            else if (statement is ForInStatement forInStatement)
            {
                GetIndenfiers(identifiers, forInStatement.Right);
                GetIndenfiers(identifiers, forInStatement.Body);
            }
            else if (statement is IfStatement ifStatement)
            {
                GetIndenfiers(identifiers, ifStatement.Test);

                GetIndenfiers(identifiers, ifStatement.Consequent);
                GetIndenfiers(identifiers, ifStatement.Alternate);
            }
            else if (statement is LabelledStatement labelledStatement)
            {
                GetIndenfiers(identifiers, labelledStatement.Body);
            }
            else if (statement is ReturnStatement returnStatement)
            {
                GetIndenfiers(identifiers, returnStatement.Argument);
            }
            else if (statement is SwitchStatement switchStatement)
            {
                GetIndenfiers(identifiers, switchStatement.Discriminant);

                foreach (var item in switchStatement.Cases)
                {
                    GetIndenfiers(identifiers, item.Test);

                    foreach (var childStatement in item.Consequent)
                    {
                        GetIndenfiers(identifiers, childStatement);
                    }
                }
            }
            else if (statement is ThrowStatement throwStatement)
            {
                GetIndenfiers(identifiers, throwStatement.Argument);
            }
            else if (statement is TryStatement tryStatement)
            {
                GetIndenfiers(identifiers, tryStatement.Block);
                GetIndenfiers(identifiers, tryStatement.Finalizer);

                foreach (var item in tryStatement.GuardedHandlers)
                {
                    GetIndenfiers(identifiers, item);
                }

                foreach (var item in tryStatement.Handlers)
                {
                    GetIndenfiers(identifiers, item.Body);
                }
            }
            else if (statement is WithStatement withStatement)
            {
                GetIndenfiers(identifiers, withStatement.Object);

                GetIndenfiers(identifiers, withStatement.Body);
            }

            //else if (statement is )
            //{
            //    GetIndenfiers(identifiers, forStatement.Update);
            //    GetIndenfiers(identifiers, forStatement.Test);

            //    GetIndenfiers(identifiers, forStatement.Body);
            //}
        }

        private static void GetIndenfiers(Dictionary<string, SyntaxNodes> identifiers, Expression expression)
        {
            // Literal
            // Expression

            if (expression is Identifier identifier)
            {
                if (identifiers.ContainsKey(identifier.Name))
                {
                    identifier.Name = identifiers.Keys.First(s => string.Equals(s, identifier.Name, StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    identifiers.Add(identifier.Name, SyntaxNodes.Identifier);
                }
            }
            else if (expression is ArrayExpression arrayExpression)
            {
                foreach (var item in arrayExpression.Elements)
                {
                    GetIndenfiers(identifiers, item);
                }
            }
            else if (expression is AssignmentExpression assignmentExpression)
            {
                GetIndenfiers(identifiers, assignmentExpression.Left);
                GetIndenfiers(identifiers, assignmentExpression.Right);
            }
            else if (expression is BinaryExpression binaryExpression)
            {
                GetIndenfiers(identifiers, binaryExpression.Left);
                GetIndenfiers(identifiers, binaryExpression.Right);
            }
            else if (expression is CallExpression callExpression)
            {
                if (callExpression.Callee is MemberExpression calleeMemberExpression
                    && calleeMemberExpression.Object is Identifier calleeIdentifier
                    && calleeMemberExpression.Property is Identifier propertyIdentifier
                    && (
                        string.Equals(propertyIdentifier.Name, "join", StringComparison.InvariantCulture)
                        || string.Equals(propertyIdentifier.Name, "map", StringComparison.InvariantCulture)
                    )
                )
                {
                    if (identifiers.ContainsKey(calleeIdentifier.Name))
                    {
                        calleeIdentifier.Name = identifiers.Keys.First(s => string.Equals(s, calleeIdentifier.Name, StringComparison.InvariantCultureIgnoreCase));
                    }
                    else
                    {
                        identifiers.Add(calleeIdentifier.Name, SyntaxNodes.ArrayExpression);
                    }
                }
                else
                {
                    GetIndenfiers(identifiers, callExpression.Callee);
                }

                foreach (var item in callExpression.Arguments)
                {
                    GetIndenfiers(identifiers, item);
                }
            }
            else if (expression is NewExpression newExpression)
            {
                GetIndenfiers(identifiers, newExpression.Callee);

                foreach (var item in newExpression.Arguments)
                {
                    GetIndenfiers(identifiers, item);
                }
            }
            else if (expression is ConditionalExpression conditionalExpression)
            {
                GetIndenfiers(identifiers, conditionalExpression.Test);

                GetIndenfiers(identifiers, conditionalExpression.Consequent);
                GetIndenfiers(identifiers, conditionalExpression.Alternate);
            }
            else if (expression is FunctionExpression functionExpression)
            {
                //GetIndenfiers(identifiers, forStatement.Update);
                //GetIndenfiers(identifiers, forStatement.Test);

                //GetIndenfiers(identifiers, forStatement.Body);
            }
            else if (expression is LogicalExpression logicalExpression)
            {
                GetIndenfiers(identifiers, logicalExpression.Right);
                GetIndenfiers(identifiers, logicalExpression.Left);
            }
            else if (expression is MemberExpression memberExpression)
            {
                GetIndenfiers(identifiers, memberExpression.Object);

                //GetIndenfiers(identifiers, memberExpression.Property);
            }
            else if (expression is ObjectExpression objectExpression)
            {
                foreach (var item in objectExpression.Properties)
                {
                    GetIndenfiers(identifiers, item.Value);
                }
            }
            else if (expression is SequenceExpression sequenceExpression)
            {
                foreach (var item in sequenceExpression.Expressions)
                {
                    GetIndenfiers(identifiers, item);
                }
            }
            else if (expression is ThisExpression thisExpression)
            {
                //GetIndenfiers(identifiers, forStatement.Update);
                //GetIndenfiers(identifiers, forStatement.Test);

                //GetIndenfiers(identifiers, forStatement.Body);
            }
            else if (expression is UnaryExpression unaryExpression)
            {
                GetIndenfiers(identifiers, unaryExpression.Argument);
            }
            else if (expression is UpdateExpression updateExpression)
            {
                //GetIndenfiers(identifiers, forStatement.Update);
                //GetIndenfiers(identifiers, forStatement.Test);

                //GetIndenfiers(identifiers, forStatement.Body);
            }

            //else if (expression is )
            //{
            //    GetIndenfiers(identifiers, forStatement.Update);
            //    GetIndenfiers(identifiers, forStatement.Test);

            //    GetIndenfiers(identifiers, forStatement.Body);
            //}
        }
    }
}
