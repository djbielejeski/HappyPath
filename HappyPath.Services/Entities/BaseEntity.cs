using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyPath.Services.Entities
{
    /// <summary>
    /// Defines the contract that all entities have
    /// </summary>
    public interface IBaseEntity
    {
        long Id { get; set; }
        DateTime CreateDateTime { get; set; }
        DateTime? UpdateDateTime { get; set; }
    }

    /// <summary>
    /// The base entity contains common properties and methods to be used by all entities in the application
    /// </summary>
    public class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
            CreateDateTime = DateTime.Now;
        }

        public long Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}
