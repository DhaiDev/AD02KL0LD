namespace MoviesAPI.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsPerPage = 10;
        private readonly int maxRecordPerPage = 50;


        public int RecordPerPage { 
        
            get { return recordsPerPage; }
            set { recordsPerPage = (value > maxRecordPerPage) ? maxRecordPerPage : value ; }
        
        }


    }
}
