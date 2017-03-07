using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HckrNewsClone.Models
{
    public class SearchModel
    {
		[Display(Name = "Date: ")]
		[DataType(DataType.Date)]
		public DateTime NewsDate { get; set; } = DateTime.Today;
		public List<NewsItem> SearchResults { get; set; }		
	}

	public class NewsItem
	{
		public int? Comments { get; set; }
		public int Date { get; set; }
		public DateTime Datetime
		{
			get
			{
				DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
				dtDateTime = dtDateTime.AddSeconds(Double.Parse(Date.ToString())).ToLocalTime();
				return dtDateTime;
			}	
		}
		public string Link { get; set; }
		public string Link_Text { get; set; }	
		public int Id { get; set; }
		public int? Points { get; set; }		
		public string Source { get; set; }
	}

}
