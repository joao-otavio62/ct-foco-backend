using ct_foco_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace ct_foco_backend.Services
{
    public class VencimentoJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<VencimentoJob> _logger;

        public VencimentoJob(IServiceScopeFactory scopeFactory, ILogger<VencimentoJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await AtualizarVencidos();

                // Calcula quanto tempo falta até meia-noite
                var agora = DateTime.Now;
                var meianoite = agora.Date.AddDays(1);
                var espera = meianoite - agora;

                _logger.LogInformation("VencimentoJob: próxima execução em {Espera}", espera);
                await Task.Delay(espera, stoppingToken);
            }
        }

        private async Task AtualizarVencidos()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<CtFocoDbContext>();

                var hoje = DateTime.UtcNow.Date;

                var vencidos = await db.Members
                    .Where(m => m.Vencimento.HasValue
                             && m.Vencimento.Value.Date < hoje
                             && m.Pagamento == "pago")
                    .ToListAsync();

                if (vencidos.Any())
                {
                    foreach (var m in vencidos)
                        m.Pagamento = "pendente";

                    await db.SaveChangesAsync();
                    _logger.LogInformation("VencimentoJob: {Count} aluno(s) marcados como pendente.", vencidos.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "VencimentoJob: erro ao atualizar vencidos.");
            }
        }
    }
}