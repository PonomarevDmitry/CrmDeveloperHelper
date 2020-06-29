using Microsoft.Xrm.Sdk.Query;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using Nav.Common.VSPackages.CrmDeveloperHelper.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    public class CompareController : BaseController<IWriteToOutputAndPublishList>
    {
        public CompareController(IWriteToOutputAndPublishList iWriteToOutputAndPublishList)
            : base(iWriteToOutputAndPublishList)
        {
        }

        #region Сравнение с веб-ресурсами.

        public async Task ExecuteComparingFilesAndWebResources(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            string operation = string.Format(Properties.OperationNames.ComparingFilesAndWebResourcesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncodingAndWriteEmptyLines(connectionData, selectedFiles, out _);

                await ComparingFilesAndWebResourcesAsync(connectionData, selectedFiles, withDetails);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        #endregion Сравнение с веб-ресурсами.

        #region Добавление в список на публикацию идентичных по тексту, но не по содержанию файлов.

        public async Task ExecuteAddingIntoPublishListFilesByType(ConnectionData connectionData, CommonConfiguration commonConfig, IEnumerable<SelectedFile> selectedFiles, OpenFilesType openFilesType)
        {
            await CheckEncodingConnectFindWebResourceExecuteActionAsync(connectionData
                , Properties.OperationNames.AddingIntoPublishListFilesFormat2
                , selectedFiles
                , openFilesType
                , AddingIntoPublishListFilesByType
                , EnumDescriptionTypeConverter.GetEnumNameByDescriptionAttribute(openFilesType)
            );
        }

        private void AddingIntoPublishListFilesByType(ConnectionData connectionData, IOrganizationServiceExtented service, TupleList<SelectedFile, WebResource> listFilesToDifference)
        {
            if (listFilesToDifference.Any())
            {
                this._iWriteToOutput.AddToListForPublish(connectionData, listFilesToDifference.Select(f => f.Item1).OrderBy(f => f.FriendlyFilePath));
            }
            else
            {
                this._iWriteToOutput.WriteToOutput(connectionData, "No files for adding to Publish List.");
            }
        }

        #endregion Добавление в список на публикацию идентичных по тексту, но не по содержанию файлов.

        public async Task ExecuteComparingFilesWithWrongEncoding(ConnectionData connectionData, List<SelectedFile> selectedFiles, bool withDetails)
        {
            string operation = string.Format(Properties.OperationNames.ComparingFilesWithWrongEncodingAndWebResourcesFormat1, connectionData?.Name);

            this._iWriteToOutput.WriteToOutputStartOperation(connectionData, operation);

            try
            {
                CheckingFilesEncoding(connectionData, selectedFiles, out List<SelectedFile> filesWithoutUTF8Encoding);

                this._iWriteToOutput.WriteToOutput(connectionData, string.Empty);

                await ComparingFilesAndWebResourcesAsync(connectionData, filesWithoutUTF8Encoding, withDetails);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(connectionData, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(connectionData, operation);
            }
        }

        #region Запуск Organization Comparer.

        public void ExecuteOrganizationComparer(ConnectionConfiguration crmConfig, CommonConfiguration commonConfig)
        {
            this._iWriteToOutput.WriteToOutputStartOperation(null, Properties.OperationNames.ShowingOrganizationComparer);

            try
            {
                WindowHelper.OpenOrganizationComparerWindow(this._iWriteToOutput, crmConfig, commonConfig);
            }
            catch (Exception ex)
            {
                this._iWriteToOutput.WriteErrorToOutput(null, ex);
            }
            finally
            {
                this._iWriteToOutput.WriteToOutputEndOperation(null, Properties.OperationNames.ShowingOrganizationComparer);
            }
        }

        #endregion Запуск Organization Comparer.


    }
}