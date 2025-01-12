import RPi.GPIO as GPIO
import requests
import time
import os
import json

GPIO.setmode(GPIO.BCM)

relay_pins = {
    'relay1': 17,  # GPIO17
    'relay2': 27   # GPIO27
}

for pin in relay_pins.values():
    GPIO.setup(pin, GPIO.OUT)
    GPIO.output(pin, GPIO.LOW)

def press_button(relay_pin):
    GPIO.output(relay_pin, GPIO.HIGH)
    time.sleep(0.5)
    GPIO.output(relay_pin, GPIO.LOW)

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

            response = requests.post(url, data={'value': user_input})
            
            if response.status_code == 200:
                press_button(relay_pins['relay1'])
            elif response.status_code == 400:
                press_button(relay_pins['relay2'])
            else:
                print(f"Unexpected response code: {response.status_code}")

    except KeyboardInterrupt:
        print("Program terminated.")
    finally:
        GPIO.cleanup()

if __name__ == "__main__":
    main()