using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViaEventAssociation.Core.Tools.ObjectMapper
{
    public interface IMappingConfig<TInput, TOutput>
       where TOutput : class
       where TInput : class
    {
        public TOutput Map(TInput input);
    }
}
