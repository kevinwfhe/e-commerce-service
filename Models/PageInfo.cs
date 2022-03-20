namespace csi5112group1project_service.Models;

public class PageInfo<T>
{
  public T rows { get; set; }
  public int totalRows { get; set; }

  public PageInfo(
    T rows,
    int totalRows
  )
  {
    this.rows = rows;
    this.totalRows = totalRows;
  }

}