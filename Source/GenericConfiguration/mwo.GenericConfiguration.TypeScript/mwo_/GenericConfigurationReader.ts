/// <reference types="xrm" />
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

    public static GetBool(key: string, defaultValue: boolean): Promise<boolean> {
        return new Promise(resolve => {
            GenericConfigurationReader.Get(key).then(result => {
                let ret = defaultValue;
                if (result !== null) {
                    const val = result.mwo_value.toLowerCase()
                    if (val === "true") ret = true;
                    else if (val === "false") ret = false
                }
                resolve(ret);
            });
        });
    }

    private static Get(key: string): Promise<any> {
        return new Promise(resolve => {
            Xrm.WebApi.retrieveMultipleRecords("mwo_genericconfiguration", "$select=mwo_value,mwo_type&$filter=mwo_key eq '" + key + "'").then(result => {
                if (result.entities && result.entities.length > 0)
                    resolve(result.entities[0]);
                else
                    resolve(null);
            });
        });
    }
}