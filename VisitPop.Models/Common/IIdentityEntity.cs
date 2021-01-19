using System;
using System.Collections.Generic;
using System.Text;

namespace VisitPop.Domain.Common
{
    /// <summary>
    /// Modelo general para indicar a nuestras entidades que van a heredar el ID
    /// </summary>
    public interface IIdentityEntity
    {
        public int Id { get; set; }
    }
}
