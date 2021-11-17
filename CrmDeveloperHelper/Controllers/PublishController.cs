using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Repository;
using System;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Controllers
{
    /// <summary>
    /// Контроллер для публикации
    /// </summary>
    public class PublishController : BaseController<IWriteToOutput>
    {
        /// <summary>
        /// Конструктор контроллера для публикации
        /// </summary>
        /// <param name="iWriteToOutput"></param>
        public PublishController(IWriteToOutput iWriteToOutput)
            : base(iWriteToOutput)
        {
        }

        #region Публикация всего.

        public async Task ExecutePublishingAll(ConnectionData connectionData)
        {
            await ConnectAndExecuteActionAsync(connectionData
                , Properties.OperationNames.PublishingAllCustomizationFormat1
                , (service) => PublishingAllAsync(service)
            );
        }

        private async Task PublishingAllAsync(IOrganizationServiceExtented service)
        {
            using (service.Lock())
            {
                try
                {
                    _iWriteToOutput.WriteToOutput(service.ConnectionData
                        , Properties.OutputStrings.PublishingAllFormat2
                        , service.ConnectionData.Name
                        , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)
                    );

                    var repository = new PublishActionsRepository(service);

                    await repository.PublishAllXmlAsync();

                    _iWriteToOutput.WriteToOutput(service.ConnectionData
                        , Properties.OutputStrings.PublishingAllCompletedFormat2
                        , service.ConnectionData.Name
                        , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)
                    );
                }
                catch (Exception ex)
                {
                    _iWriteToOutput.WriteErrorToOutput(service.ConnectionData, ex);

                    _iWriteToOutput.WriteToOutput(service.ConnectionData
                        , Properties.OutputStrings.PublishingAllFailedFormat2
                        , service.ConnectionData.Name
                        , DateTime.Now.ToString("G", System.Globalization.CultureInfo.CurrentCulture)
                    );
                }
            }
        }

        #endregion Публикация всего.
    }
}
