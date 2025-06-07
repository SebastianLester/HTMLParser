using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLParser
{
    public class BidProposalItem
    {
        public string LineNumber { get; set; }
        public string ItemNumber { get; set; }
        public string Description{ get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }

        public BidProposalItem(string lineNumber, string itemNumber, string description, string quantity, string unit)
        {
            LineNumber = lineNumber;
            ItemNumber = itemNumber;
            Description = description;
            Quantity = quantity;
            Unit = unit;
        }
        public override string ToString()
        {
            return $@"
                LineNumber = {LineNumber}
                ItemNumber = {ItemNumber}
                Description = {Description}
                Quantity = {Quantity}
                Unit = {Unit}
             ";
         }
    }
}