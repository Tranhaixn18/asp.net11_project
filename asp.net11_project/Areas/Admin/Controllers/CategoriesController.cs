using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net11_project.Areas.Admin.Attributes;
using Microsoft.Extensions.Configuration;//đọc fiel appsetting.json
using System.Data;//sử dụng đối tượng Datatable,DataSet,DataAdapter
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;//sử dụng đối tựng IFormCollection

namespace asp.net11_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class CategoriesController : Controller
    {
        //khai báo chuỗi kết nối
        string strDBConnectString = "";
        public CategoriesController (IConfiguration configuration)
        {
            strDBConnectString = configuration.GetConnectionString("DBConnectString");
        }
        public IActionResult Index()
        {
            //quy định số bản ghi trên một trang
            double recordPerPage = 3;
            double totalRecord = GetTotailRecord();
            //Math.Ceiling:hàm làm tròn các giá trị lên  vd:2.1=3,2.6=3
            ViewBag.numPage = Math.Ceiling(totalRecord / recordPerPage);
            //lấy biến page từ url
            int page = !string.IsNullOrEmpty(Request.Query["page"]) ? Convert.ToInt32(Request.Query["page"]) - 1 : 0;
            //lấy từ bản ghi nào
            int from = Convert.ToInt32(recordPerPage) * page;
            DataTable dt = new DataTable();
            using(SqlConnection con= new SqlConnection(strDBConnectString))
            {
                //offset và fetch là các tùy chọn của mệnh đề order by
                //order by (thuộc tính) + asc/desc + offset +a+ row/rows fetch next +b+ rows only
                //a: là số bản ghi bị bỏ qua
                //b: là bản ghi được thực hiện sau khi mệnh để offset được sử lý
                // rows only: chỉ b bảng ghi được thực hiện
                SqlCommand comd = new SqlCommand("select * from Categories where ParentID=0 order by ID desc offset "+ from +" rows fetch next "+recordPerPage +" rows only", con);
                SqlDataAdapter adap = new SqlDataAdapter(comd);
                adap.Fill(dt);
            }
            return View("Index",dt);
        }
        //trả về tổng số bản ghi
        public double GetTotailRecord()
        {
            DataTable dt = new DataTable();
            using(SqlConnection con = new SqlConnection(strDBConnectString))
            {
                SqlCommand comd = new SqlCommand("select ID from Categories where ParentID=0",con);
                SqlDataAdapter adap = new SqlDataAdapter(comd);
                adap.Fill(dt);
            }
            return dt.Rows.Count;
        }
        public IActionResult Update(int? id)
        {
            int _id = id ?? 0;
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(strDBConnectString))
            {
                SqlCommand comd = new SqlCommand("select * from Categories where ID="+_id, con);
                SqlDataAdapter adap = new SqlDataAdapter(comd);
                adap.Fill(dt);
            }
            return View("CreateUpdate", dt.Rows[0]);
        }
        [HttpPost]
        public IActionResult Update(IFormCollection fc,int? id)
        {
            int _id = id ?? 0;
            string nameNew = fc["Name"].ToString().Trim();
            int parentID = Convert.ToInt32(fc["parentID"].ToString());
            using(SqlConnection con= new SqlConnection(strDBConnectString))
            {
                con.Open();
                SqlCommand comd = new SqlCommand("update categories set Name=@paraName,ParentID=@paraParentID where ID=@paraID ", con);
                comd.Parameters.AddWithValue("paraName", nameNew);
                comd.Parameters.AddWithValue("paraID", _id.ToString());
                comd.Parameters.AddWithValue("paraParentID", parentID.ToString());
                comd.ExecuteNonQuery();
                con.Close();
            }
            return Redirect("/Admin/Categories");
        }
        //create
        public IActionResult Create()
        {
            return View("CreateUpdate");
        }
        [HttpPost]
        public IActionResult Create(IFormCollection fc)
        {
            string nameNew = fc["Name"].ToString().Trim();
            int parentID = Convert.ToInt32(fc["parentID"].ToString());
            using (SqlConnection con = new SqlConnection(strDBConnectString))
            {
                con.Open();
                SqlCommand comd = new SqlCommand("insert into Categories(ParentID,Name) values(@paraParentID,@paraName)", con);
                comd.Parameters.AddWithValue("paraName", nameNew);
                comd.Parameters.AddWithValue("paraParentID", parentID.ToString());
                comd.ExecuteNonQuery();
                con.Close();
            }
            return Redirect("/Admin/Categories");
        }
        public IActionResult Delete(int? id)
        {
            int _id = id ?? 0;
           
            using (SqlConnection con = new SqlConnection(strDBConnectString))
            {
                con.Open();
                //xóa danh mục cha và các danh mục con nằm trong nó
                SqlCommand comd = new SqlCommand("delete from categories where ID=@paraID or ParentID=@paraID", con);
                
                comd.Parameters.AddWithValue("paraID", _id.ToString());
                comd.ExecuteNonQuery();
                con.Close();
            }
            return Redirect("/Admin/Categories");
        }
    }
}
