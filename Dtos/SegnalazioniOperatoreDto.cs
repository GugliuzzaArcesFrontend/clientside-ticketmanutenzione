namespace clientside_ticketmanutenzione.Dtos;

public class SegnalazioniOperatoreDto
{
    public int Id { get; set; }
    
    public DateTime DataSegnalazione { get; set; }

    public string Descrizione { get; set; } = string.Empty;

   // public virtual Stato Stato { get; set; } = new Stato();

    public string Luogo { get; set; } = string.Empty;

    public string Tipologia { get; set; } = string.Empty;

    public bool AzioniAttive {  get; set; }

}
