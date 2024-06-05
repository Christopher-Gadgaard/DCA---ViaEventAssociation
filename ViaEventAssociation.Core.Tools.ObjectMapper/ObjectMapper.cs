using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ViaEventAssociation.Core.Tools.ObjectMapper
{
    public abstract class ObjectMapper(IServiceProvider serviceProvider) : IMapper
    {
        public TOutput Map<TOutput>(object input) where TOutput : class
        {
            Type type = typeof(IMappingConfig<,>).MakeGenericType(input.GetType(), typeof(TOutput));
            dynamic mappingConfig = serviceProvider.GetService(type)!;
            if (mappingConfig != null)
            {
                return mappingConfig.Map((dynamic)input);
            }

            string toJson = JsonSerializer.Serialize(input);
            return JsonSerializer.Deserialize<TOutput>(toJson)!;
        }
    }
}
