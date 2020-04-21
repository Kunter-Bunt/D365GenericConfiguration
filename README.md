# D365GenericConfiguration
This project provides an easy to use configuration entity for Dynamics 365 Customer Engagement.  
It is designed to be a quick way of allowing simpler configurations for custom extensions that do not break the barrier to justify creating a custom configuration entity.
Yet it provides an implementation of quality of live features and is well covered with tests, making it more robust than most configuration entities in the wild.

## Features
- A configuration entity that holds key, value pairs.
  - Optional description to help operators.
  - Optional types JSON, XML, Bool, Number, comma separated list, semicolon separated list. These will trigger validation, making sure that good values are entered.
- Three security roles for reading (global access), writing (basic access) and administration (global access).
- A DAL webresource (mwo_/GenericConfigurationReader.js) to use for querying the configuration entity.
  - includes optional caching in window.sessionStorage or window.localStorage.
- A DAL sample for C# to integrate to your plugins project.
  - includes caching in MemoryCache.
- A custom workflow activity for retrieving a value by key.

## Usage
Check the wiki on how to use the entity, roles and DALs.

## License
**MIT**  
