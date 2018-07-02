using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    /// <summary>
    /// Результат сравнения контента текстовых файлов
    /// </summary>
    public class ContentCopareResult
    {
        public bool IsEqual { get; private set; }

        public ReadOnlyCollection<Diff> Differences { get; private set; }

        public int Inserts { get; private set; }

        public int InsertLength { get; private set; }

        public int Deletes { get; private set; }

        public int DeleteLength { get; private set; }

        public bool IsOnlyInserts
        {
            get
            {
                return this.Inserts > 0 && this.Deletes == 0;
            }
        }

        public bool IsOnlyDeletes
        {
            get
            {
                return this.Inserts == 0 && this.Deletes > 0;
            }
        }

        public bool IsComplexChanges
        {
            get
            {
                return this.Inserts > 0 && this.Deletes > 0;
            }
        }

        public bool IsMirror { get; private set; }

        public bool IsMirrorWithInserts { get; private set; }

        public bool IsMirrorWithDeletes { get; private set; }

        public bool IsMirrorWithComplex { get; private set; }

        public ContentCopareResult(bool isEqual, IList<Diff> differences)
        {
            this.IsEqual = isEqual;

            this.Inserts = 0;
            this.InsertLength = 0;
            this.Deletes = 0;
            this.DeleteLength = 0;

            if (differences != null)
            {
                this.Differences = new ReadOnlyCollection<Diff>(differences);

                var ins = this.Differences.Where(d => d.operation == Operation.INSERT);
                var del = this.Differences.Where(d => d.operation == Operation.DELETE);

                this.Inserts = ins.Count();
                this.InsertLength = ins.Any() ? ins.Sum(d => d.text.Length) : 0;

                this.Deletes = del.Count();
                this.DeleteLength = del.Any() ? del.Sum(d => d.text.Length) : 0;

                AnalyzeDiffs();
            }
        }

        private void AnalyzeDiffs()
        {
            if (this.Inserts == 0 || this.Deletes == 0)
            {
                return;
            }

            var differences = this.Differences.Where(d => d.operation != Operation.EQUAL).ToList();

            bool hasChanges = false;

            bool hasMirror = false;

            do
            {
                hasChanges = false;

                Diff first = null;
                Diff mirror = null;

                for (int index = 0; index < differences.Count; index++)
                {
                    var item = differences[index];

                    var invertOp = InvertOperation(item.operation);

                    Diff mirrorItem = differences.Skip(index + 1).FirstOrDefault(d => d.operation == invertOp && string.Equals(d.text, item.text));

                    if (mirrorItem != null)
                    {
                        first = item;
                        mirror = mirrorItem;
                        break;
                    }
                }

                if (first != null && mirror != null)
                {
                    hasMirror = true;
                    hasChanges = true;

                    differences.Remove(first);
                    differences.Remove(mirror);
                }

            } while (hasChanges);

            if (hasMirror)
            {
                if (differences.Count == 0)
                {
                    this.IsMirror = true;
                }
                else if (differences.All(d => d.operation == Operation.INSERT))
                {
                    this.IsMirrorWithInserts = true;
                }
                else if (differences.All(d => d.operation == Operation.DELETE))
                {
                    this.IsMirrorWithDeletes = true;
                }
                else
                {
                    this.IsMirrorWithComplex = true;
                }
            }
        }

        private Operation InvertOperation(Operation operation)
        {
            switch (operation)
            {
                case Operation.DELETE:
                    return Operation.INSERT;

                case Operation.INSERT:
                    return Operation.DELETE;

                case Operation.EQUAL:
                default:
                    return Operation.EQUAL;
            }
        }

        internal string GetDescription()
        {
            if (this.IsOnlyInserts)
            {
                return "Inserts Only";
            }

            if (this.IsOnlyDeletes)
            {
                return "Deletes Only";
            }

            if (this.IsMirror)
            {
                return "Mirror changes";
            }

            if (this.IsMirrorWithInserts)
            {
                return "Mirror changes and inserts";
            }

            if (this.IsMirrorWithDeletes)
            {
                return "Mirror changes and deletes";
            }

            return string.Empty;
        }

        public string GetCompareDescription()
        {
            string result = string.Empty;

            if (this.IsOnlyInserts)
            {
                result = string.Format("(inserts +{0}(+{1}))", this.Inserts, this.InsertLength);
            }
            else if (this.IsOnlyDeletes)
            {
                result = string.Format("(deletes -{0}(-{1}))", this.Deletes, this.DeleteLength);
            }
            else
            {
                var name = string.Empty;

                if (this.IsMirror)
                {
                    name = "mirror ";
                }
                else if (this.IsMirrorWithInserts)
                {
                    name = "mirror and inserts ";
                }
                else if (this.IsMirrorWithDeletes)
                {
                    name = "mirror and deletes ";
                }

                result = string.Format("({4}+{0}(+{1})   -{2}(-{3}))"
                    , this.Inserts, this.InsertLength
                    , this.Deletes, this.DeleteLength
                    , name
                    );
            }

            return result;
        }
    }
}