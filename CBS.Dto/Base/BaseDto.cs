using System;
using System.Collections.Generic;
using System.Text;

namespace CBS.Dto.Base
{
    public class BaseDto
    {
        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
