const fs = require('fs');
const path = require('path');
const zlib = require('zlib');
const { Transform } = require('stream');
const { pipeline } = require('stream/promises');

const inputFile = path.join(__dirname, 'sample.txt');
const outputFile = path.join(__dirname, 'output.txt');
const gzipFile = path.join(__dirname, 'sample.txt.gz');
const transformedFile = path.join(__dirname, 'transformed.txt');

// Create sample file
fs.writeFileSync(inputFile, 'A'.repeat(10000) + '\n' + 'B'.repeat(10000) + '\n' + 'C'.repeat(10000));

async function demonstrateReadable() {
  console.log('--- Readable Stream ---');
  const readStream = fs.createReadStream(inputFile, { encoding: 'utf8', highWaterMark: 1024 });
  let chunkCount = 0;
  for await (const chunk of readStream) {
    chunkCount++;
  }
  console.log(`Read ${chunkCount} chunks of 1KB each`);
}

async function demonstratePipeline() {
  console.log('\n--- Pipeline (copy file) ---');
  await pipeline(
    fs.createReadStream(inputFile),
    fs.createWriteStream(outputFile)
  );
  const stats = fs.statSync(outputFile);
  console.log(`Copied ${stats.size} bytes`);
}

async function demonstrateTransform() {
  console.log('\n--- Transform Stream (uppercase) ---');
  const upperTransform = new Transform({
    transform(chunk, encoding, callback) {
      this.push(chunk.toString().toUpperCase());
      callback();
    }
  });

  await pipeline(
    fs.createReadStream(inputFile, { encoding: 'utf8' }),
    upperTransform,
    fs.createWriteStream(transformedFile)
  );
  const firstLine = fs.readFileSync(transformedFile, 'utf8').split('\n')[0];
  console.log('First line (uppercased):', firstLine);
}

async function demonstrateCompression() {
  console.log('\n--- Compression with zlib ---');
  await pipeline(
    fs.createReadStream(inputFile),
    zlib.createGzip(),
    fs.createWriteStream(gzipFile)
  );
  const originalSize = fs.statSync(inputFile).size;
  const compressedSize = fs.statSync(gzipFile).size;
  console.log(`Original: ${originalSize} bytes, Compressed: ${compressedSize} bytes`);
}

async function demonstrateDecompression() {
  console.log('\n--- Decompression ---');
  const decompressedFile = path.join(__dirname, 'decompressed.txt');
  await pipeline(
    fs.createReadStream(gzipFile),
    zlib.createGunzip(),
    fs.createWriteStream(decompressedFile)
  );
  const size = fs.statSync(decompressedFile).size;
  console.log(`Decompressed: ${size} bytes`);
}

async function main() {
  await demonstrateReadable();
  await demonstratePipeline();
  await demonstrateTransform();
  await demonstrateCompression();
  await demonstrateDecompression();

  // Cleanup
  for (const f of [inputFile, outputFile, gzipFile, transformedFile, path.join(__dirname, 'decompressed.txt')]) {
    if (fs.existsSync(f)) fs.unlinkSync(f);
  }
  console.log('\nCleanup complete');
}

main().catch(console.error);
