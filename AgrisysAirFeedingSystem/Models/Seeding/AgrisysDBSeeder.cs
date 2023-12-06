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


        var areas = new List<Area>();

        areas.Add(new Area
        {
            Name = "test"
        });


        context.Areas.AddRange(areas);

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
            Area = areas[0],
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
            Area = areas[0],
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
            Name = "blower_tmp"
        });

        context.Sensors.Add(new Sensor
        {
            Entity = blower,
            Name = "blower_RPM"
        });

        context.Sensors.Add(new Sensor
        {
            Entity = blower,
            Name = "blower_pressure"
        });

        var distributor = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen Distributor",
            Group = kitchen
        };

        context.Entities.Add(distributor);

        context.Sensors.Add(new Sensor
        {
            Entity = distributor,
            Name = "distributor_mixing",
            min = 0,
            max = 1
        });

        context.Sensors.Add(new Sensor
        {
            Entity = distributor,
            Name = "distributor_weight"
        });


        var mixer = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen Mixer",
            Group = kitchen
        };

        context.Entities.Add(mixer);

        context.Sensors.Add(new Sensor
        {
            Entity = mixer,
            Name = "mixer_mixing",
            min = 0,
            max = 1
        });

        context.Sensors.Add(new Sensor
        {
            Entity = mixer,
            Name = "mixer_weight"
        });


        var hatch1 = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "kitchen_hatch",
            Group = kitchen
        };

        context.Entities.Add(hatch1);

        context.Sensors.Add(new Sensor
        {
            Entity = hatch1,
            Name = "hatch_status"
        });

        var cellsluice = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen_Cellsluice",
            Group = kitchen
        };

        context.Entities.Add(cellsluice);

        context.Sensors.Add(new Sensor
        {
            Entity = cellsluice,
            Name = "cellsluice_rpm"
        });

        var doser = new Entity
            {
                Name = "kitchen_doser",
                Group = kitchen,
                EntityType = EntityType.Distribute
            };
        
        context.Add(doser);

        context.Add(new Sensor
        {
            Entity = doser,
            Name = "doser_RPM",
            max = 200,
            min = 0
        });
    }
}