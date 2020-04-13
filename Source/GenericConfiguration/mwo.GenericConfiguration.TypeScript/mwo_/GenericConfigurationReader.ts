export default class GenericConfigurationReader {
    public static GetString(key: string, defaultValue: string): Promise<string> {
        return new Promise(resolve => {
            GenericConfigurationReader.Get(key).then(result => {
                if (result !== null)
                    resolve(result.mwo_value);
                else
                    resolve(defaultValue);
            });
        });
    }

    private static Get(key: string): Promise<any> {
        return new Promise(resolve => {
            Xrm.WebApi.retrieveMultipleRecords("mwo_genericconfiguration", "$select=mwo_value,mwo_type&$filter=mwo_key eq '" + key + "'").then(result => {
                if (result.entities.length > 0)
                    resolve(result.entities[0]);
                else
                    resolve(null);
            });
        });
    }
}