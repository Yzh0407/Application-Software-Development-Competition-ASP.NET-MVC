namespace BookManager.Models;
public class BookModel
{
    public required int borrow_id { get; set; }
    public required string book_tile { get; set; }
    public required string stu_num { get; set; }
    public required string creat_date { get; set; }
    public required string remark { get; set; }
}