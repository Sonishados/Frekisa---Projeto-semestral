//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Frekisa.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Contrato
    {
        public int id_contrato { get; set; }
        public Nullable<int> id_cliente { get; set; }
        public Nullable<int> id_funcionario { get; set; }
        public string Plano { get; set; }
        public string parcelas { get; set; }
        public string valor_parcelas { get; set; }
        public string valor_recebido { get; set; }
    
        public virtual Clientes Clientes { get; set; }
        public virtual Funcionarios Funcionarios { get; set; }
    }
}