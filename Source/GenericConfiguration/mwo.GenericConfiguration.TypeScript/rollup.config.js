import typescript from 'rollup-plugin-typescript';
//import resolve from 'rollup-plugin-node-resolve';
import commonjs from 'rollup-plugin-commonjs';

export default {
    input: {
        GenericConfigurationReader: 'mwo_/GenericConfigurationReader.ts'
    },
    output: {
        dir: 'mwo_',
        format: 'cjs',
        name: 'main'
    },
    plugins: [
        //resolve(),
        typescript(),
        commonjs()
    ]
}