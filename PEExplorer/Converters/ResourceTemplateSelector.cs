using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PEExplorer.Core;

namespace PEExplorer.Converters {
    class ResourceTemplateSelector : DataTemplateSelector {
        public DataTemplate DefaultTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            var resourceId = (ResourceID)item;
            if(resourceId.IsStandard) {
                DataTemplate template;
                template = ((FrameworkElement)container).TryFindResource($"ResourceType{resourceId.Id}") as DataTemplate;
                if(template != null)
                    return template;
                if(DefaultTemplate != null)
                    return DefaultTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}
