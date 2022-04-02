namespace csi5112group1project_service.Models;

public class PageInfo<T>
{
  public T rows { get; set; }
  public long totalRows { get; set; }

  public PageInfo(
    T rows,
    long totalRows
  )
  {
    this.rows = rows;
    this.totalRows = totalRows;
  }

}