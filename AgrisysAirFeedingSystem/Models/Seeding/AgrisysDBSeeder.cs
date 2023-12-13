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
        
        SeedSettingsData(context);
        SeedLiveDataSensor(context);
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
     
        Group kitchenGroup = new() { GroupType = GroupType.Kitchen };
        
        Kitchen kitchen = new()
        {
            Name = "kitchen0",
            Group = kitchenGroup
        };
        context.Groups.Add(kitchenGroup);
        context.Kitchens.Add(kitchen);

        var blower = new Entity
        {
            EntityType = EntityType.Blower,
            Name = kitchen.Name+"_blower0",
            Group = kitchenGroup
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
            Name = blower.Name+"_rpm",
            SensorType = SensorType.Flow
        });

        context.Sensors.Add(new Sensor
        {
            Entity = blower,
            Name = blower.Name+"_pressure",
            SensorType = SensorType.Pressure
        });

        context.Sensors.Add(new Sensor
        {
            Entity = blower,
            Name = blower.Name+"_tmp",
            SensorType = SensorType.Temperature
        });

        var distributor = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = kitchen.Name+"_distributor0",
            Group = kitchenGroup
        };
        
        context.Sensors.Add(new Sensor
        {
            Entity = distributor,
            Name = distributor.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });

        context.Entities.Add(distributor);

        context.Sensors.Add(new Sensor
        {
            Entity = distributor,
            Name = distributor.Name+"_weight",
            SensorType = SensorType.Weight
        });


        var mixer = new Entity
        {
            EntityType = EntityType.Mixer,
            Name = kitchen.Name+"_mixer0",
            Group = kitchenGroup
        };
        
        context.Sensors.Add(new Sensor
        {
            Entity = mixer,
            Name = mixer.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });

        context.Entities.Add(mixer);

        context.Sensors.Add(new Sensor
        {
            Entity = mixer,
            Name = mixer.Name+"_mixing",
            min = 0,
            max = 1
        });

        context.Sensors.Add(new Sensor
        {
            Entity = mixer,
            Name = mixer.Name+"_weight",
            SensorType = SensorType.Weight
        });


        var hatch1 = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = kitchen.Name+"_hatch0",
            Group = kitchenGroup
        };

        context.Entities.Add(hatch1);
        
        context.Sensors.Add(new Sensor
        {
            Entity = hatch1,
            Name = hatch1.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });

        var cellsluice = new Entity
        {
            EntityType = EntityType.Distribute,
            Name = kitchen.Name+"_cellsluice0",
            Group = kitchenGroup
        };

        context.Entities.Add(cellsluice);

        context.Sensors.Add(new Sensor
        {
            Entity = cellsluice,
            Name = cellsluice.Name+"_status",
            SensorType = SensorType.Status,
            min = 0,
            max = Enum.GetValues(typeof(SensorStatus)).Length
        });
        
        var doser = new Entity
            {
                Name = kitchen.Name+"_doser0",
                Group = kitchenGroup,
                EntityType = EntityType.Distribute
            };
        
        context.Add(doser);

        context.Add(new Sensor
        {
            Entity = doser,
            Name = doser.Name+"_rpm",
            max = 200,
            min = 0,
            SensorType = SensorType.Rotation
        });
        context.Add(new Sensor
        {
            Entity = doser,
            Name = doser.Name+"_status",
            max = Enum.GetValues(typeof(SensorStatus)).Length,
            min = 0,
            SensorType = SensorType.Status  
        });
    }
}