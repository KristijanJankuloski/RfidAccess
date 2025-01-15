import RPi.GPIO as GPIO
import requests
import time
import os
import json

GPIO.setmode(GPIO.BCM)

relay_pins = {
    'relay1': 17,  # GPIO17
    'relay2': 27,  # GPIO27
    'relay3': 22   # GPIO22
}

for pin in relay_pins.values():
    GPIO.setup(pin, GPIO.OUT)
    GPIO.output(pin, GPIO.HIGH)

def press_button(relay_pin, delay):
    GPIO.output(relay_pin, GPIO.LOW)
    time.sleep(delay)
    GPIO.output(relay_pin, GPIO.HIGH)

def load_config():
    config_path = os.path.join(os.path.dirname(__file__), 'config.json')
    with open(config_path, 'r') as config_file:
        config = json.load(config_file)
    return config['url']

def main():
    try:
        url = load_config()
        while True:
            user_input = input("Code: ")
            try:
                # Send POST request
                response = requests.post(url, data=json.dumps({'value': user_input}), headers={'Content-Type': 'application/json'})
                
                if response.status_code == 200:
                    press_button(relay_pins['relay1'], 0.5)
                elif response.status_code == 400:
                    press_button(relay_pins['relay2'], 1)
                elif response.status_code == 201:
                    press_button(relay_pins['relay3'], 0.5)
                else:
                    print(f"Unexpected response code: {response.status_code}")
            except requests.exceptions.ConnectionError:
                print("Failed to connect to the server. Retrying...")
            except Exception as e:
                print(f"An error occurred: {e}")

    except KeyboardInterrupt:
        print("Program terminated.")
    finally:
        GPIO.cleanup()

if __name__ == "__main__":
    main()