using AgrisysAirFeedingSystem.Controllers;
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
        
        var hello = SeedSettingsData(context);
        SeedLiveDataSensor(context,hello);
        context.SaveChanges();
    }

    private static Group SeedSettingsData(AgrisysDbContext context)
    {
        if (context.Silos.Any()) return context.Groups.First();

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
        
        var kitchen = new Group { GroupType = GroupType.Kitchen };
        
        context.Kitchens.Add(new Kitchen
        {
            Name = "Kitchen0",
            Group = kitchen
        });

        return kitchen;
    }

    private static void SeedLiveDataSensor(AgrisysDbContext context,Group kitchen)
    {
        if (context.Groups.Any()) return;
     
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
            Name = blower.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });
        
        context.Sensors.Add(new Sensor
        {
            Entity = blower,
            Name = "Kitchen0_Blower0_RPM",
            SensorType = SensorType.Temperature
        });

        context.Sensors.Add(new Sensor
        {
            Entity = blower,
            Name = "RPM"
        });

        var distributer = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen0_Distribute0",
            Group = kitchen
        };
        
        context.Sensors.Add(new Sensor
        {
            Entity = distributer,
            Name = distributer.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });

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
            EntityType = EntityType.Mixer,
            Name = "Kitchen0_Mixer0",
            Group = kitchen
        };
        
        context.Sensors.Add(new Sensor
        {
            Entity = Mixer,
            Name = Mixer.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });

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
            Name = "Kitchen0_Hatch0",
            Group = kitchen
        };

        context.Entities.Add(Hatch1);
        
        context.Sensors.Add(new Sensor
        {
            Entity = Hatch1,
            Name = Hatch1.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });

        context.Sensors.Add(new Sensor
        {
            Entity = Hatch1,
            Name = "status"
        });

        var Cellulose = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = "Kitchen0_Cellulose0",
            Group = kitchen
        };

        context.Entities.Add(Cellulose);

        context.Sensors.Add(new Sensor
        {
            Entity = Cellulose,
            Name = Cellulose.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });
        
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