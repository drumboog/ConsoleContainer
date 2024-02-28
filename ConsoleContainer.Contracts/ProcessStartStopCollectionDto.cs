namespace ConsoleContainer.Contracts
{
    public class ProcessStartStopCollectionDto
    {
        public IEnumerable<ProcessStartStopDto> Processes { get; set; } = Enumerable.Empty<ProcessStartStopDto>();
    }
}
