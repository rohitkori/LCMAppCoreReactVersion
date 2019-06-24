import React, { Component } from 'react';
import { Picker,TextInput,Text, View, StyleSheet, Button } from 'react-native';

import t from 'tcomb-form-native';

const Form = t.form.Form;

const User = t.struct({
    algorithm: t.String,
    inputs: t.String
 
});

const formStyles = {
    ...Form.stylesheet,
    formGroup: {
        normal: {
            marginBottom: 10
        },
    },
    controlLabel: {
        normal: {
            color: 'black',
            fontSize: 18,
            marginBottom: 7,
            fontWeight: '600'
        },
        // the style applied when a validation error occours
        error: {
            color: 'red',
            fontSize: 18,
            marginBottom: 7,
            fontWeight: '600'
        }
    }
}

const options = {
    fields: {
        algorithm: {
            error: 'Algorithm is required'
        },
        inputs: {
            error: 'Enter comma separated numbers'
        },
       
    },
    stylesheet: formStyles,
};

export default class App extends Component {
    constructor(props) {
        super(props);

        this.state = {
            timeComplexity: '',
            spaceComplexity: '',
            result: ''
        };
    }

    handleSubmit = () => {
        const value = this._form.getValue();
        console.log('value: ', value);
    }

    render() {
        return (
            <View style={styles.container}>
                <Form
                    ref={c => this._form = c}
                    type={User}
                    options={options}
                />


                <Picker>
                    <Picker.Item label="Java" value="java" />
                    <Picker.Item label="JavaScript" value="js" />
                </Picker>

                <Text>Time: {this.state.timeComplexity}</Text>
                <Text>Space: {this.state.spaceComplexity}</Text>
                <Text>Result: {this.state.result}</Text>

                <Button
                    title="Calculate LCM"
                    onPress={this.handleSubmit}
                />
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        justifyContent: 'center',
        marginTop: 50,
        padding: 20,
        backgroundColor: '#ffffff',
    },
});
