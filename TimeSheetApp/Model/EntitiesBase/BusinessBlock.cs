﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheetApp.Model.EntitiesBase
{
    [Table("BusinessBlock")]
    public class BusinessBlock
    {
        [Key]
        public int Id { get; set; }
        public string BusinessBlockName { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is BusinessBlock && (obj as BusinessBlock).Id == this.Id)
            {
                return true;
            }
            return false;
        }
    }
}
