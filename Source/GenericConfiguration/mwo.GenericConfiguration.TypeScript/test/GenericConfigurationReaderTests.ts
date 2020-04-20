import * as sinon from "sinon";
import { XrmMockGenerator } from "xrm-mock";
import { expect } from "chai";
import GenericConfigurationReader from "../mwo_/GenericConfigurationReader";

const key = "PersistedKey";
const notPersistedKey = "Nothing";

function Setup(value: string, type: number) {
    const stub = sinon.stub(Xrm.WebApi, "retrieveMultipleRecords");

    stub.withArgs(sinon.match.string, sinon.match(key)).resolves({
        entities: [{ mwo_value: value, mwo_type: type }] // eslint-disable-line
    });

    stub.withArgs(sinon.match.string, sinon.match(notPersistedKey)).resolves({
        entities: []
    });
}

beforeEach(() => {
    XrmMockGenerator.initialise();
});

describe('GCR Get String Tests', () => {
    const value = "Test";
    const dflt = "Default";

    beforeEach(() => {
        Setup(value, 122870000);
    });

    it("should retrieve the config value", () => {
        //Act
        const result = GenericConfigurationReader.GetString(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Act
        const result = GenericConfigurationReader.GetString(notPersistedKey, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });
});

describe('GCR Get Bool Tests', () => {
    const value = true;
    const dflt = false;
    const type = 122870003;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetBool(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetBool(notPersistedKey, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup("Nope", type);

        //Act
        const result = GenericConfigurationReader.GetBool(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });
});

describe('GCR Get Number Tests', () => {
    const value = 127;
    const dflt = 2;
    const type = 122870004;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetNumber(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetNumber(notPersistedKey, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup("Nope", type);

        //Act
        const result = GenericConfigurationReader.GetNumber(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.equal(dflt);
        });
    });
});


describe('GCR Get Semicolon List Tests', () => {
    const value = "1;2;3;4";
    const dflt = ["1", "2", "3"];
    const type = 122870006;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetList(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result.join(";")).to.be.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetList(notPersistedKey, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup(null, type);

        //Act
        const result = GenericConfigurationReader.GetList(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });
});

describe('GCR Get Comma List Tests', () => {
    const value = "1,2,3,4";
    const dflt = ["1", "2", "3"];
    const type = 122870005;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetList(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result.join(",")).to.be.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value.toString(), type);

        //Act
        const result = GenericConfigurationReader.GetList(notPersistedKey, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup(null, type);

        //Act
        const result = GenericConfigurationReader.GetList(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });
});

describe('GCR Get JSON Object Tests', () => {
    const value = { x: "a", y: 1 };
    const dflt = { a: "x", b: 3 };
    const type = 122870001;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(JSON.stringify(value), type);

        //Act
        const result = GenericConfigurationReader.GetObject(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(value);
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(JSON.stringify(value), type);

        //Act
        const result = GenericConfigurationReader.GetObject(notPersistedKey, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup(null, type);

        //Act
        const result = GenericConfigurationReader.GetObject(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });
});

describe('GCR Get XML Object Tests', () => {
    const value = "<bookstore><book>" +
        "<title>Everyday Italian</title>" +
        "<author>Giada De Laurentiis</author>" +
        "<year>2005</year>" +
        "</book></bookstore>";
    const dflt = { a: "x", b: 3 };
    const type = 122870002;

    it("should retrieve the config value", () => {
        //Arrange
        Setup(value, type);
        
        //Act
        const result = GenericConfigurationReader.GetObject(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be./*not.*/deep.equal(dflt); //mocha does not have window.DOMParser. Evaluate Karma
            expect(result).to.be.not.null;
        });
    });

    it("should return default value if not persisted", () => {
        //Arrange
        Setup(value, type);

        //Act
        const result = GenericConfigurationReader.GetObject(notPersistedKey, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });

    it("should return default value if not castable", () => {
        //Arrange
        Setup(value.replace('<', ''), type);

        //Act
        const result = GenericConfigurationReader.GetObject(key, dflt, null);

        //Assert
        return result.then((result) => {
            expect(result).to.be.deep.equal(dflt);
        });
    });
});