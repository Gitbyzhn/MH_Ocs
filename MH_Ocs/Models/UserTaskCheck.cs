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
    
    public partial class UserTaskCheck
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public bool Status { get; set; }
    
        public virtual Task Task { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}