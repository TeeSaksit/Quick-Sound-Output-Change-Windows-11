using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick_Sound_Output_Change_Windows_11.Services.CoreAudioApi
{
    public struct PropertyKey
    {

        public Guid FormatId { get; set; }

        public int PropertyId { get; set; }

        public PropertyKey(Guid formatId, int propertyId)
        {
            this.FormatId = formatId;
            this.PropertyId = propertyId;
        }
    }
}