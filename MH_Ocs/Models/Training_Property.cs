//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MH_Ocs.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Training_Property
    {
        public int Id { get; set; }
        public int TrId { get; set; }
        public string Titile { get; set; }
        public string lang { get; set; }
    
        public virtual Training Training { get; set; }
    }
}
