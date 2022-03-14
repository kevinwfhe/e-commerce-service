namespace csi5112group1project_service.MockData;
using csi5112group1project_service.Models;

public class MClient
{
  public static readonly List<Client> MockClients = new List<Client> {
    new Client(
      id: "43d4e025-579b-44c6-a1ba-58b7dde1b5cf",
      username: "Daffi Ilchenko",
      password: "doI6vF",
      emailAddress: "dilchenko1@jiathis.com"
    ),
    new Client(
      id: "bb9d79e5-9070-44f3-bbbb-740404f6f4dd",
      username: "Rebeka Vanni",
      password: "PKFUXSN",
      emailAddress: "rvanni2@wix.com"
    ),
    new Client(
      id: "de712f75-599f-4064-bda0-ef24ff979af8",
      username: "Norbert Gillham",
      password: "v0yNRwqfbn",
      emailAddress: "ngillham3@webmd.com"
    ),
    new Client(
      id: "8d5a8401-9755-42b2-ba13-2db5d006c98a",
      username: "Corrianne Caveney",
      password: "cZTPYgnMrDvY",
      emailAddress: "ccaveney4@exblog.jp"
    ),
    new Client(
      id: "ac49bcaf-c4ec-4b3d-a927-3e1b23896c90",
      username: "Marlow Schole",
      password: "DDxrbxubIFg",
      emailAddress: "mschole5@tmall.com"
    )
  };
}