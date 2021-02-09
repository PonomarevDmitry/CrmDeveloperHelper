using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.UserControls.AttributeMetadataControls
{
    public interface IAttributeMetadataControl<out T> where T : AttributeMetadata
    {
        T AttributeMetadata { get; }

        //void AddAttribute(Entity entity);

        void AddChangedAttribute(Entity entity);

        event EventHandler RemoveControlClicked;
    }
}