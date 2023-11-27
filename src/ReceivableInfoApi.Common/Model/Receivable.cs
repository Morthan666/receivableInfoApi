using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReceivableInfoApi.Common.Model;

public class Receivable
{
    [Key]
    [JsonPropertyName("reference")]
    public string Reference { get; set; }
    
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; }
    
    [JsonPropertyName("issueDate")]
    public string IssueDate { get; set; }
    
    [JsonPropertyName("openingValue")]
    public decimal OpeningValue { get; set; }
    
    [JsonPropertyName("paidValue")]
    public decimal PaidValue { get; set; }
    
    [JsonPropertyName("dueDate")]
    public string DueDate { get; set; }
    
    [JsonPropertyName("closedDate")]
    public string? ClosedDate { get; set; }
    
    [JsonPropertyName("cancelled")]
    public bool? Cancelled { get; set; }
    
    [JsonPropertyName("debtorName")]
    public string DebtorName { get; set; }
    
    [JsonPropertyName("debtorReference")]
    public string DebtorReference { get; set; }
    
    [JsonPropertyName("debtorAddress1")]
    public string? DebtorAddress1 { get; set; }
    
    [JsonPropertyName("debtorAddress2")]
    public string? DebtorAddress2 { get; set; }
    
    [JsonPropertyName("debtorTown")]
    public string? DebtorTown { get; set; }
    
    [JsonPropertyName("debtorState")]
    public string? DebtorState { get; set; }
    
    [JsonPropertyName("debtorZip")]
    public string? DebtorZip { get; set; }
    
    [JsonPropertyName("debtorCountryCode")]
    public string DebtorCountryCode { get; set; }

    [JsonPropertyName("debtorRegistrationNumber")]
    public string? DebtorRegistrationNumber { get; set; }
}