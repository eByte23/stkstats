import * as puppeteer from 'puppeteer'; // v13.0.0 or later
import { HTTPResponse } from 'puppeteer';

(async () => {
  const browser = await puppeteer.launch();
  const page = await browser.newPage();
  const timeout = 5000;
  page.setDefaultTimeout(timeout);

  {
    const targetPage = page;
    await targetPage.setViewport({
      width: 756,
      height: 833
    })
  }
  {
    const targetPage = page;
    const promises: Promise<HTTPResponse | null>[] = []
    promises.push(targetPage.waitForNavigation());
    await targetPage.goto('https://gc.com/login');
    await Promise.all(promises);
  }
  {
    const targetPage = page;

    targetPage.click("#frm_login #email")
    await scrollIntoViewIfNeeded([
      [
        'aria/Email'
      ],
      [
        '#email'
      ],
      [
        'xpath///*[@id="email"]'
      ],
      [
        'pierce/#email'
      ]
    ], targetPage, timeout);
    const element = await waitForSelectors([
      [
        'aria/Email'
      ],
      [
        '#email'
      ],
      [
        'xpath///*[@id="email"]'
      ],
      [
        'pierce/#email'
      ]
    ], targetPage, { timeout, visible: true });
    await element.
    await element.click({
      offset: {
        x: 156.5546875,
        y: 22.6015625,
      },
    });
  }
  {
    const targetPage = page;
    await scrollIntoViewIfNeeded([
      [
        'aria/Email'
      ],
      [
        '#email'
      ],
      [
        'xpath///*[@id="email"]'
      ],
      [
        'pierce/#email'
      ]
    ], targetPage, timeout);
    const element = await waitForSelectors([
      [
        'aria/Email'
      ],
      [
        '#email'
      ],
      [
        'xpath///*[@id="email"]'
      ],
      [
        'pierce/#email'
      ]
    ], targetPage, { timeout, visible: true });
    const inputType = await element.evaluate(el => el.type);
    if (inputType === 'select-one') {
      await changeSelectElement(element, 'stkildabaseball@gmail.com')
    } else if ([
      'textarea',
      'text',
      'url',
      'tel',
      'search',
      'password',
      'number',
      'email'
    ].includes(inputType)) {
      await typeIntoElement(element, 'stkildabaseball@gmail.com');
    } else {
      await changeElementValue(element, 'stkildabaseball@gmail.com');
    }
  }
  {
    const targetPage = page;
    await targetPage.keyboard.down('Tab');
  }
  {
    const targetPage = page;
    await targetPage.keyboard.up('Tab');
  }
  {
    const targetPage = page;
    await scrollIntoViewIfNeeded([
      [
        '#login_password'
      ],
      [
        'xpath///*[@id="login_password"]'
      ],
      [
        'pierce/#login_password'
      ]
    ], targetPage, timeout);
    const element = await waitForSelectors([
      [
        '#login_password'
      ],
      [
        'xpath///*[@id="login_password"]'
      ],
      [
        'pierce/#login_password'
      ]
    ], targetPage, { timeout, visible: true });
    const inputType = await element.evaluate(el => el.type);
    if (inputType === 'select-one') {
      await changeSelectElement(element, 'S')
    } else if ([
      'textarea',
      'text',
      'url',
      'tel',
      'search',
      'password',
      'number',
      'email'
    ].includes(inputType)) {
      await typeIntoElement(element, 'S');
    } else {
      await changeElementValue(element, 'S');
    }
  }
  {
    const targetPage = page;
    await targetPage.keyboard.up('s');
  }
  {
    const targetPage = page;
    await scrollIntoViewIfNeeded([
      [
        '#login_password'
      ],
      [
        'xpath///*[@id="login_password"]'
      ],
      [
        'pierce/#login_password'
      ]
    ], targetPage, timeout);
    const element = await waitForSelectors([
      [
        '#login_password'
      ],
      [
        'xpath///*[@id="login_password"]'
      ],
      [
        'pierce/#login_password'
      ]
    ], targetPage, { timeout, visible: true });
    const inputType = await element.evaluate(el => el.type);
    if (inputType === 'select-one') {
      await changeSelectElement(element, 'Saints1879$')
    } else if ([
      'textarea',
      'text',
      'url',
      'tel',
      'search',
      'password',
      'number',
      'email'
    ].includes(inputType)) {
      await typeIntoElement(element, 'Saints1879$');
    } else {
      await changeElementValue(element, 'Saints1879$');
    }
  }
  {
    const targetPage = page;
    const promises = [];
    promises.push(targetPage.waitForNavigation());
    await scrollIntoViewIfNeeded([
      [
        'aria/Sign In'
      ],
      [
        '#login'
      ],
      [
        'xpath///*[@id="login"]'
      ],
      [
        'pierce/#login'
      ]
    ], targetPage, timeout);
    const element = await waitForSelectors([
      [
        'aria/Sign In'
      ],
      [
        '#login'
      ],
      [
        'xpath///*[@id="login"]'
      ],
      [
        'pierce/#login'
      ]
    ], targetPage, { timeout, visible: true });
    await element.click({
      offset: {
        x: 34,
        y: 14.6015625,
      },
    });
    await Promise.all(promises);
  }
  {
    const targetPage = page;
    await scrollIntoViewIfNeeded([
      [
        'aria/Email'
      ],
      [
        '#email'
      ],
      [
        'xpath///*[@id="email"]'
      ],
      [
        'pierce/#email'
      ]
    ], targetPage, timeout);
    const element = await waitForSelectors([
      [
        'aria/Email'
      ],
      [
        '#email'
      ],
      [
        'xpath///*[@id="email"]'
      ],
      [
        'pierce/#email'
      ]
    ], targetPage, { timeout, visible: true });
    await element.click({
      offset: {
        x: 157.5546875,
        y: 11.6015625,
      },
    });
  }
  {
    const targetPage = page;
    await scrollIntoViewIfNeeded([
      [
        'aria/Email'
      ],
      [
        '#email'
      ],
      [
        'xpath///*[@id="email"]'
      ],
      [
        'pierce/#email'
      ]
    ], targetPage, timeout);
    const element = await waitForSelectors([
      [
        'aria/Email'
      ],
      [
        '#email'
      ],
      [
        'xpath///*[@id="email"]'
      ],
      [
        'pierce/#email'
      ]
    ], targetPage, { timeout, visible: true });
    const inputType = await element.evaluate(el => el.type);
    if (inputType === 'select-one') {
      await changeSelectElement(element, 'stkildabaseball@gmail.com')
    } else if ([
      'textarea',
      'text',
      'url',
      'tel',
      'search',
      'password',
      'number',
      'email'
    ].includes(inputType)) {
      await typeIntoElement(element, 'stkildabaseball@gmail.com');
    } else {
      await changeElementValue(element, 'stkildabaseball@gmail.com');
    }
  }
  {
    const targetPage = page;
    await targetPage.keyboard.down('Tab');
  }
  {
    const targetPage = page;
    await targetPage.keyboard.up('Tab');
  }
  {
    const targetPage = page;
    await scrollIntoViewIfNeeded([
      [
        '#login_password'
      ],
      [
        'xpath///*[@id="login_password"]'
      ],
      [
        'pierce/#login_password'
      ]
    ], targetPage, timeout);
    const element = await waitForSelectors([
      [
        '#login_password'
      ],
      [
        'xpath///*[@id="login_password"]'
      ],
      [
        'pierce/#login_password'
      ]
    ], targetPage, { timeout, visible: true });
    const inputType = await element.evaluate(el => el.type);
    if (inputType === 'select-one') {
      await changeSelectElement(element, 'Saints1879!')
    } else if ([
      'textarea',
      'text',
      'url',
      'tel',
      'search',
      'password',
      'number',
      'email'
    ].includes(inputType)) {
      await typeIntoElement(element, 'Saints1879!');
    } else {
      await changeElementValue(element, 'Saints1879!');
    }
  }
  {
    const targetPage = page;
    const promises = [];
    promises.push(targetPage.waitForNavigation());
    await targetPage.keyboard.down('Enter');
    await Promise.all(promises);
  }
  {
    const targetPage = page;
    await targetPage.keyboard.up('Enter');
  }

  await browser.close();

  async function waitForSelectors(selectors, frame, options) {
    for (const selector of selectors) {
      try {
        return await waitForSelector(selector, frame, options);
      } catch (err) {
        console.error(err);
      }
    }
    throw new Error('Could not find element for selectors: ' + JSON.stringify(selectors));
  }

  async function scrollIntoViewIfNeeded(selectors, frame, timeout) {
    const element = await waitForSelectors(selectors, frame, { visible: false, timeout });
    if (!element) {
      throw new Error(
        'The element could not be found.'
      );
    }
    await waitForConnected(element, timeout);
    const isInViewport = await element.isIntersectingViewport({ threshold: 0 });
    if (isInViewport) {
      return;
    }
    await element.evaluate(element => {
      element.scrollIntoView({
        block: 'center',
        inline: 'center',
        behavior: 'auto',
      });
    });
    await waitForInViewport(element, timeout);
  }

  async function waitForConnected(element, timeout) {
    await waitForFunction(async () => {
      return await element.getProperty('isConnected');
    }, timeout);
  }

  async function waitForInViewport(element, timeout) {
    await waitForFunction(async () => {
      return await element.isIntersectingViewport({ threshold: 0 });
    }, timeout);
  }

  async function waitForSelector(selector, frame, options) {
    if (!Array.isArray(selector)) {
      selector = [selector];
    }
    if (!selector.length) {
      throw new Error('Empty selector provided to waitForSelector');
    }
    let element = null;
    for (let i = 0; i < selector.length; i++) {
      const part = selector[i];
      if (element) {
        element = await element.waitForSelector(part, options);
      } else {
        element = await frame.waitForSelector(part, options);
      }
      if (!element) {
        throw new Error('Could not find element: ' + selector.join('>>'));
      }
      if (i < selector.length - 1) {
        element = (await element.evaluateHandle(el => el.shadowRoot ? el.shadowRoot : el)).asElement();
      }
    }
    if (!element) {
      throw new Error('Could not find element: ' + selector.join('|'));
    }
    return element;
  }

  async function waitForElement(step, frame, timeout) {
    const {
      count = 1,
      operator = '>=',
      visible = true,
      properties,
      attributes,
    } = step;
    const compFn = {
      '==': (a, b) => a === b,
      '>=': (a, b) => a >= b,
      '<=': (a, b) => a <= b,
    }[operator];
    await waitForFunction(async () => {
      const elements = await querySelectorsAll(step.selectors, frame);
      let result = compFn(elements.length, count);
      const elementsHandle = await frame.evaluateHandle((...elements) => {
        return elements;
      }, ...elements);
      await Promise.all(elements.map((element) => element.dispose()));
      if (result && (properties || attributes)) {
        result = await elementsHandle.evaluate(
          (elements, properties, attributes) => {
            for (const element of elements) {
              if (attributes) {
                for (const [name, value] of Object.entries(attributes)) {
                  if (element.getAttribute(name) !== value) {
                    return false;
                  }
                }
              }
              if (properties) {
                if (!isDeepMatch(properties, element)) {
                  return false;
                }
              }
            }
            return true;

            function isDeepMatch(a, b) {
              if (a === b) {
                return true;
              }
              if ((a && !b) || (!a && b)) {
                return false;
              }
              if (!(a instanceof Object) || !(b instanceof Object)) {
                return false;
              }
              for (const [key, value] of Object.entries(a)) {
                if (!isDeepMatch(value, b[key])) {
                  return false;
                }
              }
              return true;
            }
          },
          properties,
          attributes
        );
      }
      await elementsHandle.dispose();
      return result === visible;
    }, timeout);
  }

  async function querySelectorsAll(selectors, frame) {
    for (const selector of selectors) {
      const result = await querySelectorAll(selector, frame);
      if (result.length) {
        return result;
      }
    }
    return [];
  }

  async function querySelectorAll(selector, frame) {
    if (!Array.isArray(selector)) {
      selector = [selector];
    }
    if (!selector.length) {
      throw new Error('Empty selector provided to querySelectorAll');
    }
    let elements = [];
    for (let i = 0; i < selector.length; i++) {
      const part = selector[i];
      if (i === 0) {
        elements = await frame.$$(part);
      } else {
        const tmpElements = elements;
        elements = [];
        for (const el of tmpElements) {
          elements.push(...(await el.$$(part)));
        }
      }
      if (elements.length === 0) {
        return [];
      }
      if (i < selector.length - 1) {
        const tmpElements = [];
        for (const el of elements) {
          const newEl = (await el.evaluateHandle(el => el.shadowRoot ? el.shadowRoot : el)).asElement();
          if (newEl) {
            tmpElements.push(newEl);
          }
        }
        elements = tmpElements;
      }
    }
    return elements;
  }

  async function waitForFunction(fn, timeout) {
    let isActive = true;
    const timeoutId = setTimeout(() => {
      isActive = false;
    }, timeout);
    while (isActive) {
      const result = await fn();
      if (result) {
        clearTimeout(timeoutId);
        return;
      }
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    throw new Error('Timed out');
  }

  async function changeSelectElement(element, value) {
    await element.select(value);
    await element.evaluateHandle((e) => {
      e.blur();
      e.focus();
    });
  }

  async function changeElementValue(element, value) {
    await element.focus();
    await element.evaluate((input, value) => {
      input.value = value;
      input.dispatchEvent(new Event('input', { bubbles: true }));
      input.dispatchEvent(new Event('change', { bubbles: true }));
    }, value);
  }

  async function typeIntoElement(element, value) {
    const textToType = await element.evaluate((input, newValue) => {
      if (
        newValue.length <= input.value.length ||
        !newValue.startsWith(input.value)
      ) {
        input.value = '';
        return newValue;
      }
      const originalValue = input.value;
      input.value = '';
      input.value = originalValue;
      return newValue.substring(originalValue.length);
    }, value);
    await element.type(textToType);
  }
})().catch(err => {
  console.error(err);
  process.exit(1);
});
