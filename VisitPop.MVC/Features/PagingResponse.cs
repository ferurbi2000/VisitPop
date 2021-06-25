using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Wrappers;

namespace VisitPop.MVC.Features
{
    public class PagingResponse<T> where T : class
    {
        public List<T> Items { get; set; }
        public MetaData Metadata { get; set; }

        public string Filters { get; set; }
        public string SortOrder { get; set; }

    }
}
