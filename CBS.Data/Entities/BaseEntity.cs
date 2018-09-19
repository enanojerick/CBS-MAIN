﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CBS.Data.Entities
{
    public partial class BaseEntity
    {
        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
