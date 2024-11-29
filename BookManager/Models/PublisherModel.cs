namespace BookManager.Models;
public class PublisherModel
{
    public required int publisher_num { get; set; }
    public required string publisher_name { get; set; }
    public required string publisher_contact { get; set; }
    public required string publisher_place { get; set; }
}


// -- 创建表
// CREATE TABLE Publishers (
//     publisher_num INT PRIMARY KEY,       -- 出版商编号，整型，作为主键
//     publisher_name VARCHAR(255) NOT NULL, -- 出版商名称，字符串类型，不能为空
//     publisher_contact VARCHAR(255) NOT NULL, -- 出版商联系人的信息，字符串类型，不能为空
//     publisher_place VARCHAR(255) NOT NULL -- 出版商的完整地址，字符串类型，不能为空
// );

// -- 插入数据
// INSERT INTO Publishers (publisher_num, publisher_name, publisher_contact, publisher_place)
// VALUES 
// (1, '清华大学出版社', '李明, 138-0013-0000', '北京市朝阳区XX路XX号'),
// (2, '人民出版社', '张华, 139-0022-0000', '上海市浦东新区XX路XX号'),
// (3, '机械工业出版社', '王伟, 137-0033-0000', '广州市天河区XX街XX号');