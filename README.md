<h1 align="center">Gameframe.ServiceProvider 👋</h1>
<p>
  <img alt="Version" src="https://img.shields.io/badge/version-1.0.0-blue.svg?cacheSeconds=2592000" />
  <a href="https://twitter.com/Cory Leach">
    <img alt="Twitter: coryleach" src="https://img.shields.io/twitter/follow/coryleach.svg?style=social" target="_blank" />
  </a>
</p>

<p>  
A simple service provider implementation for use in Unity3D.  
Provides singleton and transient services.   
</p>  

<p>  
Because the focus was on creating a simplified service provider this package does not do dependency graphs, property or constructor injection.  
It is probably most easily used as a glorified singleton manager.  
</p>

## Quick Package Install

#### Using UnityPackageManager (for Unity 2019.3 or later)
Open the package manager window (menu: Window > Package Manager)<br/>
Select "Add package from git URL...", fill in the pop-up with the following link:<br/>
https://github.com/coryleach/RepositoryName.git#1.0.0<br/>

#### Using UnityPackageManager (for Unity 2019.1 or later)

Find the manifest.json file in the Packages folder of your project and edit it to look like this:
```js
{
  "dependencies": {
    "com.gameframe.serviceprovider": "https://github.com/coryleach/RepositoryName.git#1.0.0",
    ...
  },
}
```

<!-- DOC-START -->

## Usage

### Configure your ServiceProvider and ServiceCollection
> By Default ServiceProvider.Current and ServiceCollection.Current are
> implemented using the BasicServiceProvider class that is provided so you
> can immediately use ServiceCollection.Current and ServiceProvider.Current
> right out of the box without any setup.

> //ServiceProvider is used to get service instance(s)
> // MyServiceProvider implements IServiceProvider
> ServiceProvider.Current = MyServiceProvider;
>
> //ServiceCollection handles adding which services are provided and how they will be configured (Singleton vs Transient)
> // MyServiceCollection implements IServiceCollection
> ServiceCollection.Current = MyServiceCollection;

### Adding Singleton Service
> //This will configure a specific instance which already has been created
> ServiceCollection.Current.AddSingleton(serviceInstance.GetType(),serviceInstance);
>
> //You can also configure a service to be served when requesting a parent type
> ServiceCollection.Current.AddSingleton(typeof(ParentClass),childClassServiceInstance);
>
> //You can also configure a function that will be used to construct the singleton service on demand
> ServiceCollection.Current.AddSingleton((provider)=> new MyService());

### Adding Transient Service
> //Transient services require a factory because a new instance is created every time
> ServiceCollection.Current.AddTransient<ServiceType>((provider) => new ServiceType());
>
> //Adding a transient service with a parent type
> ServiceCollection.Current.AddTransient<ParentType,ServiceType>((provider) => (ParentType)new ServiceType());

### Getting a Service
> //Get a particular service
> var service = ServiceProvider.Get<ServiceType>();
>
> //If more than one service of the given type is provided we can get them all
> var services = ServiceProvider.GetAll<ServiceType>();

### Using the Bootstrapper

> The UnityServiceProviderBootstrapper can be used to configure singleton services from MonoBehaviours or ScriptableObjects
> In Awake() UnityServiceProviderBootstrapper will configure the ServiceCollection and ServiceProvider to an instance of UnityServiceProvider
> Additional services can be configured by creating a child class of UnityServiceProviderBootstrapper
> and overriding the ConfigureServices method. Be sure to include a call to base.ConfigureServices(unityServiceProvider) or the singletonService list will not be added.

<!-- DOC-END -->

## Author

👤 **Cory Leach**

* Twitter: [@coryleach](https://twitter.com/coryleach)
* Github: [@coryleach](https://github.com/coryleach)


## Show your support

Give a ⭐️ if this project helped you!

***
_This README was generated with ❤️ by [Gameframe.Packages](https://github.com/coryleach/unitypackages)_
