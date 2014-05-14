using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace MvcWebRole.Models
{
    public class MailingList:TableEntity
    {
        public MailingList()
        {
            this.RowKey = "mailinglist";
        }

        [Required]
        [RegularExpression(@"[\w]+",
         ErrorMessage = @"Only alphanumeric characters and underscore (_) are allowed.")]
        [Display(Name = "Name")]
        public string ListName
        {
            get
            {
                return this.PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
            }
        }

        [Required]
        [Display(Name = "Email ID")]
        public string FromEmailAddress { get; set; }

        public string Description { get; set; }
    }


     
}