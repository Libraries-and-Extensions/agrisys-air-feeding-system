using AgrisysAirFeedingSystem.Models.DB;
using AgrisysAirFeedingSystem.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace AgrisysAirFeedingSystem.Models.Seeding;

public class AgrisysDBSeeder
{
    public static void Seed(IApplicationBuilder app)
    {
        var context = app.ApplicationServices.CreateScope().ServiceProvider
            .GetRequiredService<AgrisysDbContext>();

        if (context == null) throw new Exception("AgrisysDbContext is null");

        if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();

        SeedLiveDataSensor(context);
        SeedSettingsData(context);
        context.SaveChanges();
    }

    private static void SeedSettingsData(AgrisysDbContext context)
    {
        if (context.Silos.Any()) return;

        var silos = new List<Silo>();
        //populate Silo, Target, FeedingTime, Area, Mixture tables

        silos.Add(new Silo
        {
            Capacity = 30000,
            AlarmMin = 8000,
            AfterRun = 200,
            TransferSpeed = 100,
            MixingTime = 10
        });

        silos.Add(new Silo
        {
            Capacity = 20000,
            AlarmMin = 3000,
            AfterRun = 100,
            TransferSpeed = 100,
            MixingTime = 10,
            AlternativeSilo = silos[0]
        });

        context.Silos.AddRange(silos);

        var mixtures = new List<Mixture>();

        mixtures.Add(new Mixture
        {
            FirstSilo = silos[0],
            SecondSilo = silos[1]
        });
        mixtures.Add(new Mixture
        {
            FirstSilo = silos[0],
            SecondSilo = silos[1]
        });

        context.Mixtures.AddRange(mixtures);


        var Areas = new List<Area>();

        Areas.Add(new Area
        {
            Name = "test"
        });


        context.Areas.AddRange(Areas);

        var setPoints = new List<MixtureSetpoint>();

        setPoints.Add(new MixtureSetpoint
        {
            Mixture = mixtures[0],
            SetPoint = 70,
            age = 1
        });

        //mixture one
        setPoints.Add(new MixtureSetpoint
        {
            Mixture = mixtures[0],
            SetPoint = 85,
            age = 20
        });

        setPoints.Add(new MixtureSetpoint
        {
            Mixture = mixtures[0],
            SetPoint = 100,
            age = 30
        });

        //mixture two
        setPoints.Add(new MixtureSetpoint
        {
            Mixture = mixtures[1],
            SetPoint = 85,
            age = 20
        });

        setPoints.Add(new MixtureSetpoint
        {
            Mixture = mixtures[1],
            SetPoint = 100,
            age = 30
        });

        context.MixtureSetpoints.AddRange(setPoints);

        var targets = new List<Target>();

        targets.Add(new Target
        {
            PressureAlarm = 100,
            BlowerStrength = 100,
            Cellesluse = 100,
            BeforeOpen = 100,
            BeforeClose = 100,
            pigCount = 5,
            pigAge = 2,
            Area = Areas[0],
            Mixture = mixtures[0]
        });

        targets.Add(new Target
        {
            PressureAlarm = 100,
            BlowerStrength = 100,
            Cellesluse = 100,
            BeforeOpen = 100,
            BeforeClose = 100,
            pigCount = 10,
            pigAge = 2,
            Area = Areas[0],
            Mixture = mixtures[1]
        });

        context.Target.AddRange(targets);
    }

    private static void SeedLiveDataSensor(AgrisysDbContext context)
    {
        if (context.Groups.Any()) return;

        var kitchen = new Group { GroupType = GroupType.Kitchen };
        context.Groups.Add(kitchen);

        var blower = new Entity
        {
            EntityType = EntityType.Blower,
            Name = "Kitchen Blower",
            Group = kitchen
        };

        context.Entities.Add(blower);


        context.Sensors.Add(new Sensor
        {
            Entity = blower,
            Name = "tmp"
        });

        context.Sensors.Add(new Sensor
        {
            Entity = blower,
            Name = "RPM"
        });

        var distributer = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen Distributer",
            Group = kitchen
        };

        context.Entities.Add(distributer);

        context.Sensors.Add(new Sensor
        {
            Entity = distributer,
            Name = "fill"
        });

        context.Sensors.Add(new Sensor
        {
            Entity = distributer,
            Name = "weight"
        });


        var Mixer = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen Mixer",
            Group = kitchen
        };

        context.Entities.Add(Mixer);

        context.Sensors.Add(new Sensor
        {
            Entity = Mixer,
            Name = "fill"
        });

        context.Sensors.Add(new Sensor
        {
            Entity = Mixer,
            Name = "weight"
        });


        var Hatch1 = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen_Hatch1",
            Group = kitchen
        };

        context.Entities.Add(Hatch1);

        context.Sensors.Add(new Sensor
        {
            Entity = Hatch1,
            Name = "status"
        });

        var Cellulose = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen_Cellulose",
            Group = kitchen
        };

        context.Entities.Add(Cellulose);

        context.Sensors.Add(new Sensor
        {
            Entity = Cellulose,
            Name = "RPM"
        });

        context.Sensors.Add(new Sensor
        {
            Entity = Cellulose,
            Name = "TMP"
        });
    }
}