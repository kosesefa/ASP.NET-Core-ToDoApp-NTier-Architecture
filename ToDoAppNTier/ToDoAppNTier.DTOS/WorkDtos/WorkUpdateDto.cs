 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoAppNTier.DTOS.Interfaces;

namespace ToDoAppNTier.DTOS.WorkDtos
{
    public class WorkUpdateDto:IDto
    {

        public int Id { get; set; }
       
        public string Definition { get; set; }
        public bool IsCompleted { get; set; }
    }
}
