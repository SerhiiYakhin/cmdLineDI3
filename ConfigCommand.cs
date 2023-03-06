using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;

namespace ConsoleApp;

public class ConfigCommand : Command
{
    public ConfigCommand()
        : base("config", "This command is used to set configuration options.")
    {
        AddCommand(new SetEnvironmentCommand());
        AddCommand(new SetRoleCommand());
        AddCommand(new SetUserCommand());
        AddCommand(new CurrentConfigCommand());
    }
}

public class ConfigurationCommandBinder : BinderBase<IConfigurationService>
{
    /// <inheritdoc/>
    protected override IConfigurationService GetBoundValue(BindingContext bindingContext)
    {
        return bindingContext.GetRequiredService<IConfigurationService>();
    }
}

public class SetEnvironmentCommand : Command
{
    public SetEnvironmentCommand()
        : base("set-environment")
    {
        var envArgument = new Argument<EnvironmentType>(
            "environment",
            //() => EnvironmentType.Test,
            "The environment to set."
        )
        {
            Arity = ArgumentArity.ExactlyOne
        };
        AddArgument(envArgument);

        this.SetHandler<EnvironmentType, IConfigurationService>(
            async (environment, configurationService) =>
            {
                await configurationService.SetEnvironmentAsync(environment);
            },
            envArgument,
            new ConfigurationCommandBinder()
        );
    }
}

public class SetRoleCommand : Command
{
    public SetRoleCommand()
        : base("set-role")
    {
        var roleArgument = new Argument<RoleType>(
            name: "globalRole",
            //() => RoleType.GlobalAdmin,
            description: "The user Global Role to set."
        )
        {
            Arity = ArgumentArity.ExactlyOne
        };
        AddArgument(roleArgument);

        this.SetHandler<RoleType, IConfigurationService>(
            async (role, configurationService) =>
            {
                await configurationService.SetRoleAsync(role);
            },
            roleArgument,
            new ConfigurationCommandBinder()
        );
    }
}

public class SetUserCommand : Command
{
    public SetUserCommand()
        : base("set-user")
    {
        //var emailOption = new Option<string>("--email", "The email address of the user.");
        //var passwordOption = new Option<string>("--password", "The password of the user.");


        var emailArgument = new Argument<string>(
            "email",
            "The user email, in case of use a user from Azure CLI context - no password needed."
        )
        {
            Arity = ArgumentArity.ExactlyOne
        };
        var passwordOption = new Option<string>(
            new string[] { "--password", "-p" },
            "The user password to use user-defined credentials."
        );

        AddArgument(emailArgument);
        AddOption(passwordOption);

        this.SetHandler<string, string, IConfigurationService>(
            async (email, password, configurationService) =>
            {
                await configurationService.SetUserAsync(email, password);
            },
            emailArgument,
            passwordOption,
            new ConfigurationCommandBinder()
        );
    }
}

public class CurrentConfigCommand : Command
{
    public CurrentConfigCommand()
        : base("current")
    {
        this.SetHandler<IConfigurationService>(
            async (configurationService) =>
            {
                await configurationService.PrintCurrentConfigurationAsync();
            },
            new ConfigurationCommandBinder()
        );
    }
}
