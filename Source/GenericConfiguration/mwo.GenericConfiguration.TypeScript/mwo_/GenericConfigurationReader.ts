/// <reference types="xrm" />

export default class GenericConfigurationReader {
    public static GetString(key: string, defaultValue: string): Promise<string> {
        return new Promise(resolve => {
            GenericConfigurationReader.Get(key).then(result => {
                if (result !== null && result.mwo_value !== null)
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
                if (result !== null && result.mwo_value !== null) {
                    const val = result.mwo_value.toLowerCase()
                    if (val === "true") ret = true;
                    else if (val === "false") ret = false
                }
                resolve(ret);
            });
        });
    }

    public static GetNumber(key: string, defaultValue: number): Promise<number> {
        return new Promise(resolve => {
            GenericConfigurationReader.Get(key).then(result => {
                let ret = defaultValue;
                if (result !== null && result.mwo_value !== null) {
                    const val = Number(result.mwo_value);
                    if (!Number.isNaN(val)) ret = val;
                }
                resolve(ret);
            });
        });
    }

    public static GetList(key: string, defaultValue: Array<string>): Promise<Array<string>> {
        return new Promise(resolve => {
            GenericConfigurationReader.Get(key).then(result => {
                let ret = defaultValue;
                if (result !== null && result.mwo_value !== null) {
                    const separator = result.mwo_type === 122870006/*SemicolonseparatedList*/ ? ';' : ',';
                    ret = result.mwo_value.split(separator);
                }
                resolve(ret);
            });
        });
    }

    public static GetObject(key: string, defaultValue: any): Promise<any> { // eslint-disable-line
        return new Promise(resolve => {
            GenericConfigurationReader.Get(key).then(result => {
                if (result !== null && result.mwo_value !== null) {
                    try {
                        if (result.mwo_type === 122870001/*JSON*/ || result.mwo_value.startsWith("{") || result.mwo_value.startsWith("[")) 
                            resolve(JSON.parse(result.mwo_value));

                        else if (result.mwo_type === 122870002/*XML*/ || result.mwo_value.StartsWith("<"))
                            resolve(new window.DOMParser().parseFromString(result.mwo_value, "text/xml").documentElement)

                        else resolve(defaultValue);
                    } catch (error) {
                        console.warn("Failed to generate Object: " + error);
                        resolve(defaultValue);
                    }
                }
                else resolve(defaultValue);
            });
        });
    }

    private static Get(key: string): Promise<any> {
        return new Promise(resolve => {
            Xrm.WebApi.retrieveMultipleRecords("mwo_genericconfiguration", "?$select=mwo_value,mwo_type&$filter=mwo_key eq '" + key + "'").then(result => {
                if (result.entities && result.entities.length > 0)
                    resolve(result.entities[0]);
                else
                    resolve(null);
            });
        });
    }
}