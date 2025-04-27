namespace Hj.Profile.Dto;

public sealed class ProfileOutputDto
{
  public string? FirstName { get; set; }
  public string? SurName { get; set; }
  public ICollection<string>? Identity { get; set; }
  public ICollection<string>? Claims { get; set; }
  public ICollection<string>? AuthItems { get; set; }
}
