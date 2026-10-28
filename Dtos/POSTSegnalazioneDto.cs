namespace clientside_ticketmanutenzione.Dtos
{
    public class POSTSegnalazioneDto
    {
        //public int? Id { get; set; }


        public virtual int? IdUtenza { get; set; }              //prende IdUtenza per fare riferimento all'utente che sta creando la segnalazione

        public string Luogo { get; set; } = string.Empty;       //luogo della segnalazione

        //public DateOnly Data { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public virtual int IdTipologia { get; set; }            //tipo di segnalazione

        public string Descrizione { get; set; } = string.Empty; //Descrizione della segnalazione. può anche essere anche vuota

        //public virtual int? IdAzione { get; set; }            
    }
}
