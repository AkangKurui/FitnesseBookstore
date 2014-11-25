using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNesse.models
{
    public class InvoiceReport
    {
        public Guid Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime DueDate { get; set; }
        public Customer Customer { get; set; }
        public double SubTotal { get; set; }
        public double NetTotal { get; set; }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Term Term { get; set; }
    }

    public class Term
    {
        public string Code { get; set; }
        public int Value { get; set; }
    }
}
