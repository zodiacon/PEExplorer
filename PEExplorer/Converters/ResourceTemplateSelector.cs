using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PEExplorer.Core;
using PEExplorer.ViewModels;
using PEExplorer.ViewModels.Resources;

namespace PEExplorer.Converters {
    class ResourceTemplateSelector : DataTemplateSelector {
        public DataTemplate DefaultTemplate { get; set; }
        static readonly DataTemplate _empty = new DataTemplate();

        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            if(item != null) {
                if(item is ResourceTypeViewModel)
                    return _empty;

                var resourceId = item as ResourceViewModel;
                if(resourceId != null) {
                    if(resourceId.Type.ResourceType.IsStandard) {
                        DataTemplate template;
                        template = ((FrameworkElement)container).TryFindResource($"ResourceType{resourceId.Type.ResourceType.Id}") as DataTemplate;
                        if(template != null)
                            return template;
                        if(DefaultTemplate != null)
                            return DefaultTemplate;
                    }
                    else
                        return DefaultTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
