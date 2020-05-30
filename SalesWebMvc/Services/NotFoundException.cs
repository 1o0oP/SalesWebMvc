using System;

namespace SalesWebMvc.Services
{
  public class NotFoundException : ApplicationException
  {
    public NotFoundException(string message) : base(message)
    {
    }
  }
}
